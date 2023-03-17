// Karol Weglewski s22864
using System.Text.RegularExpressions;


var url = args[0];


// W sytuacji gdy argument nie został przekazany, powinien zostać zwrócony błąd ArgumentNullException

if (args.Length == 0)
    throw new ArgumentNullException("brak url");

// Jeśli został przekazany argument, który nie jest poprawnym adresem URL, powinien zostać zwrócony błąd ArgumentException
// Do sprawdzenia poprawność adresu URL, można skorzystać z klasy Uri

if (!(Uri.IsWellFormedUriString(url, UriKind.Absolute)))
    throw new ArgumentException("nieprawidlowy url");



/*var set = new HashSet<string>();*/
var regex = new Regex(@"[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+");
var httpClient = new HttpClient();

try
{

    var httpResult = await httpClient.GetAsync(url);
    if (httpResult.IsSuccessStatusCode) // autopodpowiedziane
    {
        var httpContent = await httpResult.Content.ReadAsStringAsync();
        var matches = regex.Matches(httpContent);


        foreach (Match match in matches)
        {
            set.Add(match.Value);
        }

        //W sytuacji, gdy nie znaleziono żadnych adresów email, powinien zostać zwrócony błąd Exception z informacją Nie znaleziono adresów email
        if (matches.Count >= 1)
        {
            throw new Exception("Nie znaleziono adresów email");
        }


        foreach (var value in set)
        {
            Console.WriteLine(value);
        }

        //matches.Select(match => match.Value).Distinct().ToList().ForEach(e => Console.WriteLine(e));
    }

}
//W przypadku, gdy podczas pobierania strony wystąpi błąd (czyli status żądania, który nie jest z przedziału 200-299),
// powinien zostać zwrócony błąd Exception z informacją Błąd w czasie pobierania strony
catch (Exception e)
{
    Console.WriteLine("Błąd w czasie pobierania strony" + e.Message);
}
finally // autopodpowiedziane
{
    httpClient.Dispose();
}

