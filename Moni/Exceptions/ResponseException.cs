using Moni.Enums;
using Moni.Extensions;
using System;

namespace Moni.Exceptions
{
    [Serializable]
    internal class ResponseException : Exception
    {
        public ResponseException(ErrorCode errorCode) : base(errorCode.GetDescription())
        {
            ErrorCode = errorCode;
        }

        public ErrorCode ErrorCode { get; set; }
    }
}