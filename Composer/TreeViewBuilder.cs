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
    /// Contains methods for building TreeView nodes from directoryPath strings.
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
            // Split the path into its components
            path = path.Trim('/', '\\');
            string[] pathComponents = path.Split('/', '\\');

            // Create a node for each component in the path
            TreeNodeCollection nodeList = _nodes;
            for (int i = 0; i < pathComponents.Length; i++)
            {
                string component = pathComponents[i];

                // Only create a new node if a node doesn't already exist for the component
                TreeNode node = null;
                foreach (TreeNode child in nodeList)
                {
                    if (child.Text == component)
                    {
                        node = child;
                        break;
                    }
                }
                if (node == null)
                {
                    node = new TreeNode(component);
                    nodeList.Add(node);

                    // If this is the last path component, tag it and set its image
                    if (i == pathComponents.Length - 1)
                    {
                        node.Tag = tag;
                        node.ImageIndex = image;
                        node.SelectedImageIndex = image;
                    }
                }
                nodeList = node.Nodes;
            }
        }
    }
}
