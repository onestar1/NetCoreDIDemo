﻿using System;
using System.Collections.Generic;
using System.Text;


namespace 中间件跟服务.MyException
{
   public class SuperException:ApplicationException
    {
        public SuperException()
        {
            Log.Info(this.Message, this);
        }

        public SuperException(string message) : base(message)
        {
            Log.Info(message, this);
        }

        public SuperException(string message, Exception inner) : base(message, inner)
        {
            Log.Info(message, inner);
        }

    }
}
