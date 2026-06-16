using System;

namespace Puya.Base
{
    public class ValidationAttribute: Attribute
    {
        public bool RequiresNullCheck { get; protected set; }
    }
    public class ValidationRule
    {
        public string Property { get; set; }
        public object Value { get; set; }
    }
    /*
{
    props: {
	    lastName: [
		    { type: 'required' },
		    { type: 'minlen', minlen: 3 }
	    ],

    },
    order: [
	    'firstname',
	    'lastname',
	    'age',
	    'dob'
    ]
}
     */
    public class ValidationRules
    {
        public ValidationRules(string rules)
        {
            
        }
    }
}
