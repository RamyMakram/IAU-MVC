// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
using IAU.DTO.Entity;
using Model.DTO;
using Newtonsoft.Json;
using System.Collections.Generic;

public class Root
{
	[JsonProperty("$id")]
	public string Id { get; set; }
	public bool success { get; set; }
	public Result result { get; set; }
}
public class GeniricReciever
{
	[JsonProperty("$id")]
	public string Id { get; set; }
	public bool success { get; set; }
	public string result { get; set; }
}
public class Result
{
	[JsonProperty("$id")]
	public string Id { get; set; }
	public List<SelectList_DTO> Countries { get; set; }
	public List<SelectList_DTO> type { get; set; }
	public List<SelectList_DTO> titles { get; set; }
	public List<SelectList_DTO> nationalty { get; set; }
	public List<SelectList_DTO> Regions { get; set; }
	public List<SelectList_DTO> Cities { get; set; }
	public List<SelectList_DTO> doctype { get; set; }
	public List<SelectList_DTO> provider { get; set; }
	public List<SelectList_DTO> mainServices { get; set; }
	public List<SelectList_DTO> supporteddocs { get; set; }
	public List<SelectList_DTO> subServices { get; set; }
	public List<SelectListItemDto> ServiceType { get; set; }
	public List<SelectListItemDto> RequestType { get; set; }
}
public class mainServices : SelectList_DTO
{
	[JsonProperty("$id")]
	public string Id { get; set; }

}



