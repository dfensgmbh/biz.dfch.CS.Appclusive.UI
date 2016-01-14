using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Tree
{
    /// <summary>
    /// see http://jonmiles.github.io/bootstrap-treeview/
    /// https://github.com/jonmiles/bootstrap-treeview
    /// </summary>
    public class Node
    {
        public Node()
        {
            this.nodes = new List<Node>();
            this.tags = new List<string>();
        }

        public string text { get; set; }
        public string icon { get; set; }
        public string selectedIcon { get; set; }
        public string color { get; set; }
        public string backColor { get; set; }
        public string href { get; set; }
        public bool selectable { get; set; }
        public State state { get; set; }
        public List<string> tags { get; set; }
        public List<Node> nodes { get; set; }
    }
}