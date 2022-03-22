using System;
using System.IO;
using Godot;
using Newtonsoft.Json;

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

    public Vector2 ComboPosition;

    public Vector2 AccuracyPosition;
    public string AccuracyFormat;

    public Texture[] JudgeImage;
    public Rect2 JudgeRect;
    public Vector2 ErrorBarPosition;

    public Skin(SkinLoader.SkinSettings settings, SkinLoader skinLoader, string skinFolder)
    {
        _settings = settings;

        _defaultResolution = new Vector2(1280, 720);
        InputMode = _settings.InputMode;

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
            Notes[i].Texture = skinLoader.LoadImage(skinFolder, _settings.NoteImage, i);
            Keys[i].Texture = skinLoader.LoadImage(skinFolder, _settings.KeyImage, i);
            PressedKeys[i].Texture = skinLoader.LoadImage(skinFolder, _settings.KeyPressedImage, i);
        }

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

        public float[] ComboPosition;

        public float[] AccuracyPosition;
        public string AccuracyFormat;

        public string[] JudgeImage;
        public float[] JudgePosition;
        public float[] JudgeScale;

        public float[] ErrorBarPosition;
    }

    public Skin Build(int inputMode, string skinFolder, string skinFile)
    {
        SkinSettings skinSettings;
        using (StreamReader file = System.IO.File.OpenText(skinFolder + skinFile))
        {
            JsonSerializer serializer = new JsonSerializer();
            skinSettings = (SkinSettings)serializer.Deserialize(file, typeof(SkinSettings));
        }

        if (skinSettings.InputMode != inputMode)
        {
            throw new Exception("InputMode from skin is not exact as in loaded chart");
        }
        
        Skin skin = new Skin(skinSettings, this, skinFolder);
        return skin;
    }

    public Vector2 ArrToVec(float[] array)
    {
        return new Vector2(array[0], array[1]);
    }

    public ImageTexture LoadImage(string skinFolder, string[] namesArray, int index) 
    {   
        ImageTexture image = new ImageTexture();
        image.Load(skinFolder + namesArray[index]);
        return image;
    }
}