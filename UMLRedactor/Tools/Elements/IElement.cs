﻿using UMLRedactor.Additions;

namespace UMLRedactor.Tools.Elements
{
    public interface IElement
    {
        Enums.ElementTypes Type { get; set; }
    }
}