using System.Collections;
using System.Windows.Forms;
using BESM3CA.Model;

namespace BESM3CA.UI
{
    // Create a node sorter that implements the IComparer interface.
    public class NodeSorter : IComparer
    {
        // Compare the length of the strings, or the strings
        // themselves, if they are the same length.
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;

            if (tx.Tag == null && ty.Tag == null)
                return string.Compare(tx.Text, ty.Text);

            if (tx.Tag == null)
                return int.MaxValue;

            if (ty.Tag == null)
                return int.MinValue;

            // Compare the length of the strings, returning the difference.
            if (((NodeData)tx.Tag).NodeOrder != ((NodeData)ty.Tag).NodeOrder)
                return (((NodeData)tx.Tag).NodeOrder - ((NodeData)ty.Tag).NodeOrder);

            // If they are the same length, call Compare.
            return string.Compare(tx.Text, ty.Text);
        }
    }
}