namespace BulletBallet.utils.NucleusFW.Utils;

/// <summary>
/// Class that contains all variables and methods useful between projects
/// </summary>
public static class Nucleus_Nodes
{
    /// <summary>
    /// Recurcive method to get the 1st Node in a Group named pGroupName (Editor > Node window > Groups)
    /// </summary>
    /// <param name="node">The node to check</param>
    /// <param name="groupName">the name of the Group</param>
    /// <returns>If found, the node in the Group; if not found, null</returns>
    public static Node FindNode_BasedOnGroup(Node node, string groupName)
    {
        // Recursive on the parent node (until all nodes read or the node found)
        if(node != null && !node.IsInGroup(groupName))
            return FindNode_BasedOnGroup(node.GetParent(), groupName);

        return node;
    }
}