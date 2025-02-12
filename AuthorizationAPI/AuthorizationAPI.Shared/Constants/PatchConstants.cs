using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationAPI.Shared.Constants
{
    public class PatchConstants
    {
        public static string AddOperation { get; } = "add";
        public static string RemoveOperation { get; } = "remove";
        public static string ReplaceOperation { get; } = "replace";
        public static string CopyOperation { get; } = "copy";
        public static string MoveOperation { get; } = "move";
        public static string TestOperation { get; } = "test";
    }
}
