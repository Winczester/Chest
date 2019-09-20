using System;
using System.Collections.Generic;
using System.Text;

namespace SenderLibrary
{
    interface ISender
    {

        void Send(string email, string subject, string message);

    }
}
