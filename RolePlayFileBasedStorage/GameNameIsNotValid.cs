﻿using System;
using System.Runtime.Serialization;

namespace RolePlayFileBasedStorage
{
    [Serializable]
    public class GameNameIsNotValid : Exception
    {
        public GameNameIsNotValid()
        {
        }

        public GameNameIsNotValid(string message) : base(message)
        {
        }

        public GameNameIsNotValid(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GameNameIsNotValid(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}