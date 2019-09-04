﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Chest.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http2;


namespace Chest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsController : Controller
    {
        
        private readonly ChestDatabaseContext _databaseContext;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly GoodsCounter _goodsCounter;
        public string MessageForEvent { get; set; }


        public GoodsController(ChestDatabaseContext context, IHostingEnvironment environment, GoodsCounter goodsCounter)
        {
            _databaseContext = context;
            _hostingEnvironment = environment;
            _goodsCounter = goodsCounter;

            if (_databaseContext.Categories.Any() == false & _databaseContext.Manufacturers.Any() == false & _databaseContext.Goods.Any() == false)
            {
                Category smartphone = new Category { Name = "Smartphone" };
                Category notebook = new Category { Name = "Notebook" };
                _databaseContext.Categories.AddRange(smartphone, notebook);

                Manufacturer apple = new Manufacturer { Name = "Apple" };
                Manufacturer xiaomi = new Manufacturer { Name = "Xiaomi" };

                _databaseContext.Manufacturers.AddRange(apple, xiaomi);

                _databaseContext.Goods.AddRange(
                    new Goods { Name = "Mi A2 Lite", Price = 5000, Category = smartphone, Manufacturer = xiaomi },
                    new Goods { Name = "Mi Notebook Pro", Price = 25000, Category = notebook, Manufacturer = xiaomi },
                    new Goods { Name = "IPhone X", Price = 20000, Category = smartphone, Manufacturer = apple },
                    new Goods { Name = "MacBook Pro", Price = 25000, Category = notebook, Manufacturer = apple });
                _databaseContext.SaveChanges();
            }

        }


        //Глава 11 :Сериализация в JSON, десериализация в GetAllGoods.cshtml
        [HttpGet]
        public IActionResult GetAllGoods()
        {
            if (_databaseContext.Goods.Any() == false)
            {
                return NotFound();
            }
            else
            {
                DataContractJsonSerializer goodsJsonSerializer = new DataContractJsonSerializer(typeof(List<Goods>));
                using (FileStream fileStream = new FileStream(@"C:\Users\user\source\repos\Chest\Chest\Goods.json", FileMode.OpenOrCreate))
                {
                    goodsJsonSerializer.WriteObject(fileStream, _databaseContext.Goods.ToList());
                }

                ViewBag.Categories = _databaseContext.Categories.ToList();
                ViewBag.Manufacturers = _databaseContext.Manufacturers.ToList();
                
                return View();
            }
        }

        //Getting goods from database by category
        [HttpGet("Category")]
        public IActionResult GetGoodsByCategory(string category)
        {
            var goods = _databaseContext.Goods.Where(tmpGoods => tmpGoods.Category.Name == category);
            if (goods == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.GoodsByCategory = goods.ToList();
                ViewBag.Category = category;
                return View();
            }
        }

        //Getting goods from database by manufacturer
        [HttpGet("Manufacturer")]
        public IActionResult GetGoodsByManufacturer(string manufacturer)
        {
            var goods = _databaseContext.Goods.Where(tmpGoods => tmpGoods.Manufacturer.Name == manufacturer);
            if (goods == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.GoodsByManufacturer = goods;
                ViewBag.Manufacturer = manufacturer;
                return View();
            }
        }

        //Глава 6: Делегаты, события и лямбда-выражения. Глава 16. Многопоточность
        [HttpPost("AddGoods")]
        public IActionResult AddGoods([FromForm]string goodsName, [FromForm] int price, [FromForm]string categoryName, [FromForm]string manufacturerName)
        {
            //if (!_databaseContext.Goods.Any(goods => goods.Name == goodsName) )
            //{
            //    _databaseContext.Goods.Add(new Goods
            //    {
            //        Name = goodsName,
            //        Price = price,
            //        Category = _databaseContext.Categories.FirstOrDefault(category => category.Name == categoryName),
            //        Manufacturer = _databaseContext.Manufacturers.FirstOrDefault(manufacturer => manufacturer.Name == manufacturerName)
            //    });
            //    _databaseContext.SaveChanges();
            //    _goodsCounter.OnAdded += Message;
                
            //}

            Thread goodsThread = new Thread(new ParameterizedThreadStart(GoodsThread));
            goodsThread.Start(new Goods
            {
                Name = goodsName,
                Price = price,
                Category = _databaseContext.Categories.FirstOrDefault(category => category.Name == categoryName),
                Manufacturer = _databaseContext.Manufacturers.FirstOrDefault(manufacturer => manufacturer.Name == manufacturerName)
            });
            goodsThread.Join();

            _goodsCounter.Added();
            return Content(MessageForEvent);
        }

        //Глава 10: Работа с потоками и файловой системой, чтение текстовых файлов
        [HttpPost()]
        public IActionResult AddGoodsFromFile(IFormFile goodsFile)
        {
            string goodsNameFromFile = null;
            string path = "/files/" + goodsFile.Name;
            using (FileStream fileStream = new FileStream(_hostingEnvironment.WebRootPath + path, FileMode.Create))
            {
                goodsFile.CopyTo(fileStream);
            }
            using (StreamReader streamReader = new StreamReader(_hostingEnvironment.WebRootPath + path, Encoding.Default))
            {
                while (!streamReader.EndOfStream)
                {
                    goodsNameFromFile = streamReader.ReadLine();
                    if (!_databaseContext.Goods.Any(tmpGoods => tmpGoods.Name == goodsNameFromFile))
                    {
                        _databaseContext.Goods.Add(new Goods
                        {
                            Name = goodsNameFromFile,
                            Price = Int32.Parse(streamReader?.ReadLine()),
                            Category = _databaseContext.Categories.FirstOrDefault(category => category.Name == streamReader.ReadLine()),
                            Manufacturer = _databaseContext.Manufacturers.FirstOrDefault(manufacturer => manufacturer.Name == streamReader.ReadLine())
                        });
                        _databaseContext.SaveChanges();
                    }
                }
                
            }
            return Redirect("~/api/Goods");
        }

        //Updating goods in database by id
        [HttpPut]
        public IActionResult UpdateGoods(int id, string goodsName, int price, string categoryName, string manufacturerName)
        {
            Goods goods = _databaseContext.Goods.FirstOrDefault(tmpGoods => tmpGoods.ID == id);
            if (goods == null)
            {
                return NotFound();
            }
            else
            {
                goods.Name = goodsName;
                goods.Price = price;
                goods.Category = _databaseContext.Categories.FirstOrDefault(category => category.Name == categoryName);
                goods.Manufacturer = _databaseContext.Manufacturers.FirstOrDefault(manufacturer => manufacturer.Name == manufacturerName);
                _databaseContext.Update(goods);
                _databaseContext.SaveChanges();
                return Ok();
            }
        }

        //Deleting goods by id
        [HttpDelete]
        public IActionResult DeleteGoods([FromQuery]int id)
        {
            Goods goods = _databaseContext.Goods.FirstOrDefault(tmpGoods => tmpGoods.ID == id);
            if(goods == null)
            {
                return NotFound();
            }
            else
            {
                _databaseContext.Goods.Remove(goods);
                _databaseContext.SaveChanges();
                return Ok();
            }
        }

        [NonAction]
        public void Message()
        {
            MessageForEvent = "New goods added";
        }

        [NonAction]
        public void GoodsThread(object goodsToAdd)
        {
            Goods tmpGoodsToAdd = (Goods) goodsToAdd;
            if (!_databaseContext.Goods.Any(goods => goods.Name == tmpGoodsToAdd.Name))
            {
                _databaseContext.Goods.Add(tmpGoodsToAdd);
                _databaseContext.SaveChanges();
                _goodsCounter.OnAdded += Message;

            }
        }

    }
}