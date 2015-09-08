using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare
{
    public class Vertex
    {
        public Vertex()
        {
        }

        public Vertex(string value)
        {
            this.value = value;
            this.isSearch = false;
        }

        //頂點的值 
        public string value
        {
            get;
            set;
        }
        //有無被搜尋過
        public bool isSearch
        {
            get;
            set;
        }
    }
}