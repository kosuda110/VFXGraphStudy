using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kosu.UnityLibrary
{
    public class BaseJsonData
    {
        public string className;

        public BaseJsonData()
        {
            className = GetType().Name;
        }
    }
}
