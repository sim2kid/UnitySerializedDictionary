using sim2kid.Package.SerializedDictionary.Runtime;
using UnityEditor;

namespace sim2kid.Package.SerializedDictionary.Editor.Drawers
{
    /// <summary>
    /// BlackboardDrawer is a custom property drawer for the Blackboard class.
    /// It extends the DictionaryDrawer class to facilitate rendering and managing
    /// the Blackboard dictionary in the Unity Editor.
    /// </summary>
    [CustomPropertyDrawer(typeof(Blackboard))]
    public class BlackboardDrawer : DictionaryDrawer<string, object> { }
}
