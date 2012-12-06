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
            string[] pathComponents = SplitPath(path);

            // Create a node for each component in the path
            TreeNodeCollection nodeList = _nodes;
            for (int i = 0; i < pathComponents.Length; i++)
            {
                string component = pathComponents[i];

                // Only create a new node if a node doesn't already exist for the component
                TreeNode node = FindChild(nodeList, component);
                if (node == null)
                {
                    node = new TreeNode(component);
                    node.Name = component;
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
                result = FindChild(nodeList, component);
                if (result == null)
                    return null; // The node with the given path doesn't exist

                nodeList = result.Nodes;
            }
            return result;
        }

        private string[] SplitPath(string path)
        {
            path = path.Trim('/', '\\');
            return path.Split('/', '\\');
        }

        private TreeNode FindChild(TreeNodeCollection nodes, string text)
        {
            // This is actually faster than TreeNodeCollection.Find()
            foreach (TreeNode child in nodes)
            {
                if (child.Text == text)
                    return child;
            }
            return null;
        }
    }
}
