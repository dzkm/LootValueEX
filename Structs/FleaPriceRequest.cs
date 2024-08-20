namespace LootValueEX.Structs
{
    internal readonly struct FleaPriceRequest
	{
		internal string TemplateID { get; }
		public FleaPriceRequest(string templateId) => TemplateID = templateId;
	}
}
