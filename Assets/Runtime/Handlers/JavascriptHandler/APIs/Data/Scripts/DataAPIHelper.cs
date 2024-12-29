using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Data
{
    public class DataAPIHelper : MonoBehaviour
    {
        /// <summary>
        /// Instance of the Entity API Helper.
        /// </summary>
        private static DataAPIHelper instance;

        public void Initialize()
        {
            instance = this;
        }
    }
}