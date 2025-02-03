namespace TrybeHotel.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrybeHotel.Dto;

// 1. Implemente as models da aplicação
public class Hotel
{
    public int HotelId { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }

    [ForeignKey("CityId")]
    public int CityId { get; set; }
    public virtual IEnumerable<Room>? Rooms { get; set; }
    public virtual City? City { get; set; }
}