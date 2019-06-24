using System;

namespace NavyBlueDtos
{
    class DtosException : Exception
    {
        public int code { get; private set; }

        public DtosException(int code, string message)
            : base(message)
        {
            this.code = code;
        }
    }
}
