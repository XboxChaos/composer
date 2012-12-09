using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Composer
{
    /// <summary>
    /// Contains methods for building sorted TreeViews from path strings.
    /// </summary>
    public class TreeViewBuilder
    {
        private TreeNodeCollection _nodes;

        /// <summary>
        /// Constructs a new TreeViewBuilder for a TreeNodeCollection.
        /// </summary>
        /// <param name="nodes">The TreeNodeCollection to add nodes to.</param>
        public TreeViewBuilder(TreeNodeCollection nodes)
        {
            _nodes = nodes;
        }

        /// <summary>
        /// Adds a node to the tree with the given path and tag.
        /// </summary>
        /// <param name="path">The full path of the new node, with components separated by slashes.</param>
        /// <param name="image">The index of the image to set for the node.</param>
        /// <param name="tag">The object to set the node's Tag to.</param>
        public void AddNode(string path, int image, object tag)
        {
            string[] pathComponents = SplitPath(path);

            // Create a node for each component in the path
            TreeNodeCollection nodeList = _nodes;
            for (int i = 0; i < pathComponents.Length; i++)
            {
                string component = pathComponents[i];

                // Only create a new node if a node doesn't already exist for the component
                int index = FindChild(nodeList, component);
                if (index < 0)
                {
                    index = ~index;

                    TreeNode node = new TreeNode(component);
                    node.Name = component;
                    nodeList.Insert(index, node);

                    // If this is the last path component, tag it and set its image
                    if (i == pathComponents.Length - 1)
                    {
                        node.Tag = tag;
                        node.ImageIndex = image;
                        node.SelectedImageIndex = image;
                    }
                }
                nodeList = nodeList[index].Nodes;
            }
        }

        /// <summary>
        /// Gets the TreeNode corresponding to a given path.
        /// </summary>
        /// <param name="path">The path of the node to retrieve.</param>
        /// <returns>The TreeNode if found, or null otherwise.</returns>
        public TreeNode GetNode(string path)
        {
            string[] pathComponents = SplitPath(path);

            // Traverse the tree, finding the node with the given name each time
            TreeNodeCollection nodeList = _nodes;
            TreeNode result = null;
            foreach (string component in pathComponents)
            {
                int index = FindChild(nodeList, component);
                if (index < 0)
                    return null; // The node with the given path doesn't exist

                nodeList = nodeList[index].Nodes;
            }
            return result;
        }

        private string[] SplitPath(string path)
        {
            path = path.Trim('/', '\\');
            return path.Split('/', '\\');
        }

        private int FindChild(TreeNodeCollection nodes, string text)
        {
            // Binary search the node list
            int start = 0;
            int end = nodes.Count - 1;
            while (start <= end)
            {
                int mid = (start + end) / 2;
                int comparison = nodes[mid].Text.CompareTo(text);
                if (comparison == 0)
                    return mid;
                else if (comparison < 0)
                    start = mid + 1;
                else
                    end = mid - 1;
            }
            
            // Not found - return the inverse of the suggested insertion index
            return ~start;
        }
    }
}
