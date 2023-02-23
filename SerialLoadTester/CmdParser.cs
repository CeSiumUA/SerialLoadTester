namespace SerialLoadTester;

public class CmdParser
{
    private List<string> _args = new();

    public CmdParser(IEnumerable<string> args)
    {
        _args = args.ToList();
    }

    public string? GetOption(string optionName)
    {
        int? index = null;
        for(int i = 0; i < _args.Count; i++)
        {
            string val = _args[i];
            if(!val.StartsWith('-'))
            {
                continue;
            }

            val = val.Replace("-", string.Empty);

            if(val == optionName)
            {
                index = i;
                break;
            }
        }

        if (!index.HasValue)
        {
            return null;
        }

        var nextVal = _args[index.Value + 1];

        if (!nextVal.StartsWith("-"))
        {
            return nextVal;
        }
        else
        {
            return string.Empty;
        }
    }
}
