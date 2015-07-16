
using T4ConsoleApp;

namespace MessageCodeGenrator
{
    public static class Messages
    {
        public static class Validation
        {
            public static MessageEntry MandatoryField = new MessageEntry("MandatoryField", "The {0} is mandatory.", "Validation");
            public static MessageEntry GreaterThan = new MessageEntry("GreaterThan", "The {0} must be greater than {1}.", "Validation");
        }
        public static class Confirmation
        {
            public static MessageEntry ReallyDelete = new MessageEntry("ReallyDelete", "Do you really want to delete the {0}.", "Confirmation");
        }
    }
}