using UnityEngine;
using ExitGames.Client.Photon;

public class MyCustomProperties
{
    public static Hashtable setColorProperties(Color _hair, Color _head, Color _body, Color _weapon)
    {
        Hashtable properties = new Hashtable();
        properties["SKIN"] = true;
        properties["SKIN_HAIR"] = ColorString.GetStringFromColor(_hair);
        properties["SKIN_HEAD"] = ColorString.GetStringFromColor(_head);
        properties["SKIN_BODY"] = ColorString.GetStringFromColor(_body);
        properties["SKIN_WEAPON"] = ColorString.GetStringFromColor(_weapon);

        return properties;
    }

    public static Hashtable setColor(string part, Color _color)
    {
        Hashtable properties = new Hashtable();
        properties["SKIN"] = true;
        properties[part] = ColorString.GetStringFromColor(_color);
        return properties;
    }

    public static Hashtable setInfoProperties(bool isSpectate, string teamName)
    {
        Hashtable properties = new Hashtable();
        properties["ISSPECTATE"] = isSpectate;
        properties["GAMEOVER"] = true;
        properties["QUALIFIED"] = true;
        properties["TEAM"] = teamName;
        return properties;
    }

    public static Hashtable setQualified(bool _qualified)
    {
        Hashtable properties = new Hashtable();
        properties["QUALIFIED"] = _qualified;
        return properties;
    }

    public static Hashtable setGameOver(bool _gameover)
    {
        Hashtable properties = new Hashtable();
        properties["GAMEOVER"] = _gameover;
        return properties;
    }
}
