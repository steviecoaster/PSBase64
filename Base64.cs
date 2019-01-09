using System.Management.Automation;
using System;

namespace Base64
{
    [Cmdlet(VerbsData.ConvertTo, "Base64String")]
    [OutputType(typeof(string))]
    public class ConvertToBase64Command : Cmdlet
    {
        // Declare the parameters for the cmdlet.
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [Alias("InputString","String","Text")]
        public string InputObject { get; set; }


        [Parameter]
        [ArgumentCompleter(typeof(EncodingCompleter))]
        [EncodingTransform]
        public System.Text.Encoding Encoding { get; set; } = EncodingTransformAttribute.StandardEncoding["UTF8"];


        //This is the "Process {}" block of a Powershell script
        protected override void ProcessRecord()
        {
            //WriteObject("Hello " + name + "!");

            byte[] bytes = Encoding.GetBytes(InputObject);
            WriteObject(Convert.ToBase64String(bytes), true);

        }
    } //class

    [Cmdlet(VerbsData.ConvertFrom, "Base64String")]
    [OutputType(typeof(string))]
    public class ConvertFromBase64Command : Cmdlet
    {
        // Declare the parameters for the cmdlet.
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [Alias("InputString","String","Text")]
        public string InputObject { get; set; }


        [Parameter]
        [ArgumentCompleter(typeof(EncodingCompleter))]
        [EncodingTransform]
        public System.Text.Encoding Encoding { get; set; } = EncodingTransformAttribute.StandardEncoding["UTF8"];


        //This is the "Process {}" block of a Powershell script
        protected override void ProcessRecord()
        {
            //WriteObject("Hello " + name + "!");
            // $Encoding.GetString([System.Convert]::FromBase64String($Base64String))
            byte[] bytes = Convert.FromBase64String(InputObject);
            WriteObject(Encoding.GetString(bytes), true);
        }
    } //class
} //namespace