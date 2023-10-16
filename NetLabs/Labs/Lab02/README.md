Бібліотека "NetLab2.dll" містить дуже малий код, тому створювати окремий проект для неї не мало сенсу.  
Якщо ж ви хочете зібрати її окремо, ось вихідний код:  
```csharp
namespace NetLab2;

public static class Lab2Lib
{
	public static int CountSpaces(string str) => str.Count(c => c == ' ');
	public static string LatinToUpper(string str)
	{
		string tmp = "";
		foreach (var c in str)
		{
			if (c >= 'a' && c <= 'z')
				tmp += (char)(c - 32);
			else tmp += c;
		}
		return tmp;
	}
}
```

Після компіляції, замініть файл "NetLab2.dll" на новий.