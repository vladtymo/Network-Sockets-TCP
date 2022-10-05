﻿using System;

namespace SharedData
{
    public enum OperationType { Add = 1, Sub, Mult, Div };

    [Serializable] 
    public class Request
    {
        public double A { get; set; }
        public double B { get; set; }
        public OperationType Operation { get; set; }
    }
}
