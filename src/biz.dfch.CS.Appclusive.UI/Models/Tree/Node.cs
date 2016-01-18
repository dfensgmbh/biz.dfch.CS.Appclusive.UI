using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Tree
{
    /// <summary>
    /// see http://wwwendt.de/tech/fancytree/doc/jsdoc/global.html#NodeData
    /// </summary>
    public class Node
    {
        public Node()
        {     
        }

        public List<Node> children { get; set; }
        public bool expanded { get; set; }
        public string extraClasses { get; set; }
        public bool folder { get; set; }
        public bool hideCheckbox { get; set; }
        //public string icon { get; set; }
        public string key { get; set; }
        public bool lazy { get; set; }
        public bool selected { get; set; }
        public string title { get; set; }
        public string tooltip { get; set; }
        public bool unselectable { get; set; }
    }
}