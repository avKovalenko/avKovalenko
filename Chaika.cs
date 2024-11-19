using System.Text.Json;
using System.Text.Json.Serialization;

public class Task1
{
    private Dictionary<long, Int16[]> reservs = new();

    public void LoadFromDbOrOtherService()
    {
        for (long i = 0; i < 220_000; i++)
        {
            reservs.Add(i, new short[330]);
        }
    }

    public void Reserve(long hotelId, DateTime reservationDate)
    {
        var totalDays = (int)(reservationDate - DateTime.Now).TotalDays;
        if (reservs.TryGetValue(hotelId, out short[]? days))
            if (totalDays < 330 && days[totalDays]>0) 
                days[totalDays]--;
    }

    public (DateTime? start, DateTime? end) GetFirstLastFreeReservDate(long hotelId)
    {
        if (reservs.TryGetValue(hotelId, out short[]? days))
        {
            var firstDayIndex = Array.FindIndex(days, x => x > 0);
            var lastDayIndex = Array.FindLastIndex(days, x => x > 0);
            var currentDay = DateTime.Now;
            if (firstDayIndex != -1 && lastDayIndex != -1)
            {
                return (currentDay.AddDays(firstDayIndex), currentDay.AddDays(-lastDayIndex));
            }
        }

        return (DateTime.MinValue, DateTime.MinValue);
        //або
        //return (null,null);
    }
    
    public DateTime[] GetReservationDates(int month)
    {
        return new DateTime[31];
    }
}
/*TASK 2*/
public record Reservation
{
    [JsonPropertyName("reservationId"),JsonRequired] public Guid ReservationId{ get; set; }
    [JsonPropertyName("guestName"),JsonRequired]     public string GuestName { get; set; }
    [JsonPropertyName("checkInDate"),JsonRequired]   public DateTime CheckInDate{ get; set; }
    [JsonPropertyName("checkOutDate"),JsonRequired]  public DateTime CheckOutDate{ get; set; }
    [JsonPropertyName("specialRequests")]            public string? SpecialRequests{ get; set; }

    public string Serialize()
    {
        return JsonSerializer.Serialize(this);
    }
    
    public string SerializeWithoutSpecialRequestsIfNull()
    {
        var option = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            
        };
        return JsonSerializer.Serialize(this, option);
    }
};