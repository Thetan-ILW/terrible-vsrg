using System;
using System.IO;
using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Skin
{
    public struct Element
    {
        public Texture Texture;
        public Vector2 Position;
        public Vector2 Size;
    }

    private SkinLoader.SkinSettings _settings;
    private Vector2 _defaultResolution;
    public Vector2 Ratio;

    public int InputMode;

    public float HitPosition;
    public float ColumnStart;
    public Vector2[] ColumnSize;

    public Element[] Notes;
    public Element[] Keys;
    public Element[] PressedKeys;

    public DrawableText AccuracyText;
    public DrawableText ComboText;
    public JudgeDrawable JudgeImage; 
    public ErrorBar ErrorBar;

    public Skin(SkinLoader.SkinSettings settings, SkinLoader skinLoader, string skinFolder)
    {
        // GENERAL
        _settings = settings;

        _defaultResolution = new Vector2(1280, 720);
        InputMode = _settings.InputMode;

        // NOTES AND KEYS
        HitPosition = _settings.HitPosition * OS.WindowSize.y;
        ColumnStart = _settings.ColumnStart;
        ColumnSize = new Vector2[InputMode];

        for (int i = 0; i != _settings.ColumnSizeX.GetLength(0); i++)
        {
            ColumnSize[i] = new Vector2(
                _settings.ColumnSizeX[i],
                _settings.ColumnSizeY[i]
            );
        }

        Notes = new Element[InputMode];
        Keys = new Element[InputMode];
        PressedKeys = new Element[InputMode];
        
        for (int i = 0; i != InputMode; i++)
        {
            Notes[i].Texture = skinLoader.LoadImage(skinFolder, _settings.NoteImage[i]);
            Keys[i].Texture = skinLoader.LoadImage(skinFolder, _settings.KeyImage[i]);
            PressedKeys[i].Texture = skinLoader.LoadImage(skinFolder, _settings.KeyPressedImage[i]);
        }

        // INFO
        string[] textAlign = _settings.AccuracyText.Align;
        Align horizontalAlign = skinLoader.GetAlign(textAlign[0]);
        Align verticalAlign = skinLoader.GetAlign(textAlign[1]);

        AccuracyText = new DrawableText(
            skinFolder + _settings.AccuracyText.FontName,
            _settings.AccuracyText.FontSize,
            _settings.AccuracyText.Format,
            horizontalAlign,
            verticalAlign
        );

        textAlign = _settings.ComboText.Align;
        horizontalAlign = skinLoader.GetAlign(textAlign[0]);
        verticalAlign = skinLoader.GetAlign(textAlign[1]);

        ComboText = new DrawableText(
            skinFolder + _settings.ComboText.FontName,
            _settings.ComboText.FontSize,
            _settings.ComboText.Format,
            horizontalAlign,
            verticalAlign
        );

        textAlign = _settings.JudgeImage.Align;
        horizontalAlign = skinLoader.GetAlign(textAlign[0]);
        verticalAlign = skinLoader.GetAlign(textAlign[1]);

        JudgeImage = new JudgeDrawable(
            skinLoader.LoadImage(skinFolder, _settings.JudgeImage.Images[0]),
            skinLoader.LoadImage(skinFolder, _settings.JudgeImage.Images[1]),
            skinLoader.LoadImage(skinFolder, _settings.JudgeImage.Images[2]),
            skinLoader.LoadImage(skinFolder, _settings.JudgeImage.Images[3]),
            _settings.JudgeImage.Scale,
            horizontalAlign,
            verticalAlign
        );

        ErrorBar = new ErrorBar(
            settings.ErrorBar.Limit,
            horizontalAlign,
            verticalAlign 
        );

        Update();
    }

    public void Update()
    {
        HitPosition = _settings.HitPosition * OS.WindowSize.y;
        Ratio = new Vector2(
            _defaultResolution.x / OS.WindowSize.x,
            _defaultResolution.y / OS.WindowSize.y
        );

        for (int i = 0; i != InputMode; i++)
        {
            float position = 0f;
            
            for(int e = 0; e != i;)
            {
                position += (Notes[e].Texture.GetSize().y / Ratio.y) * ColumnSize[e].x;
                e++;
            }

            position = position / OS.WindowSize.x;

            // Notes
            Notes[i].Position = new Vector2(
                    (_settings.ColumnStart + position) * OS.WindowSize.x,
                    0
            );

            Notes[i].Size = new Vector2(
                (Notes[i].Texture.GetSize().y / Ratio.y) * ColumnSize[i].x,
                (Notes[i].Texture.GetSize().y / Ratio.y) * ColumnSize[i].y
            );

            // Keys
            Keys[i].Position = new Vector2(
                (_settings.ColumnStart + position) * OS.WindowSize.x,
                HitPosition
            );

            Keys[i].Size = new Vector2(
                (Keys[i].Texture.GetSize().y / Ratio.y) * ColumnSize[i].x,
                (Keys[i].Texture.GetSize().y / Ratio.y) * ColumnSize[i].y
            );

            // Pressed Keys
            PressedKeys[i].Position = new Vector2(
                (_settings.ColumnStart + position) * OS.WindowSize.x,
                HitPosition
            );

            PressedKeys[i].Size = new Vector2(
                (Keys[i].Texture.GetSize().y / Ratio.y) * ColumnSize[i].x,
                (Keys[i].Texture.GetSize().y / Ratio.y) * ColumnSize[i].y
            );
        }

        Vector2 accuracyPosition = ArrToVec(_settings.AccuracyText.Position);
        AccuracyText.ExactPosition = new Vector2(
            accuracyPosition.x * OS.WindowSize.x,
            accuracyPosition.y * OS.WindowSize.y
        );
        AccuracyText.UpdatePosition();

        Vector2 comboPosition = ArrToVec(_settings.ComboText.Position);
        ComboText.ExactPosition = new Vector2(
            comboPosition.x * OS.WindowSize.x,
            comboPosition.y * OS.WindowSize.y
        );
        ComboText.UpdatePosition();

        Vector2 judgeImagePosition = ArrToVec(_settings.JudgeImage.Position);
        JudgeImage.ExactPosition = new Vector2(
            judgeImagePosition.x * OS.WindowSize.x,
            judgeImagePosition.y * OS.WindowSize.y
        );
        JudgeImage.UpdateRect();

        Vector2 errorBarPosition = ArrToVec(_settings.ErrorBar.Position);
        ErrorBar.ExactPosition = new Vector2(
            errorBarPosition.x * OS.WindowSize.x,
            errorBarPosition.y * OS.WindowSize.y
        );
        ErrorBar.UpdatePosition();
    }

    private Vector2 ArrToVec(float[] array)
    {
        return new Vector2(array[0], array[1]);
    }
}

public class SkinLoader
{
    public struct SkinSettings
    {
        public int InputMode;

        public float HitPosition;
        public float ColumnStart;
        public float[] ColumnSizeX;
        public float[] ColumnSizeY;

        public string[] NoteImage;
        public string[] KeyImage;
        public string[] KeyPressedImage;

        public TextSettings AccuracyText;
        public TextSettings ComboText;
        public JudgeImageSettings JudgeImage;
        public ErrorBarSettings ErrorBar;
    }

    public struct TextSettings
    {
        public float[] Position;
        public string FontName;
        public int FontSize;
        public string[] Align;
        public string Format;
    }

    public struct JudgeImageSettings
    {
        public float[] Position;
        public float Scale;
        public string[] Align;
        public string[] Images;
    }

    public struct ErrorBarSettings
    {
        public float[] Position;
        public int Limit;
    }

    public Skin Build(int inputMode, string skinFolder)
    {
        string fileName = "";

        string metadataFile = System.IO.File.ReadAllText(skinFolder + "metadata.json", System.Text.Encoding.Default);
        JObject data = (JObject)JsonConvert.DeserializeObject(metadataFile);
        fileName = data[inputMode.ToString()].Value<string>();
        
        SkinSettings skinSettings;
        using (StreamReader file = System.IO.File.OpenText(skinFolder + fileName))
        {
            JsonSerializer serializer = new JsonSerializer();
            skinSettings = (SkinSettings)serializer.Deserialize(file, typeof(SkinSettings));
        }

        if (skinSettings.InputMode != inputMode)
            throw new Exception("InputMode from skin is not exact as in loaded chart");
        
        Skin skin = new Skin(skinSettings, this, skinFolder);
        return skin;
    }

    public ImageTexture LoadImage(string skinFolder, string name) 
    {   
        ImageTexture image = new ImageTexture();
        image.Load(skinFolder + name);
        return image;
    }

    public Align GetAlign(string text)
    {
        Align align;
        if (!Enum.TryParse<Align>(text, out align))
            throw new Exception($"Align with name {text} is not valid");
        
        return align;
    }
}