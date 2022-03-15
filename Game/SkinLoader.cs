using System;
using System.IO;
using Godot;
using Newtonsoft.Json;

public class Skin
{
    public int InputMode;

    public Vector2 Position;
    public Vector2 NoteSize;
    public Vector2 KeySize;

    public Texture[] NoteImage;
    public Texture[] KeyImage;
    public Texture[] KeyPressedImage;

    public Vector2 ComboPosition;

    public Vector2 AccuracyPosition;
    public string AccuracyFormat;

    public Texture[] JudgeImage;
    public Rect2 JudgeRect;
    public Vector2 ErrorBarPosition;

    public Skin(SkinLoader.SkinSettings settings, SkinLoader skinLoader, string skinFolder)
    {
        InputMode = settings.InputMode;

        Position = skinLoader.ArrToVec(settings.Position);
        NoteSize = skinLoader.ArrToVec(settings.NoteSize);
        KeySize = skinLoader.ArrToVec(settings.KeySize);

        NoteImage = new Texture[settings.InputMode];
        KeyImage = new Texture[settings.InputMode];
        KeyPressedImage = new Texture[settings.InputMode];

        for (int i = 0; i != InputMode; i++)
        {
            skinLoader.LoadImage(skinFolder, settings.NoteImage, NoteImage, i);
            skinLoader.LoadImage(skinFolder, settings.KeyImage, KeyImage, i);
            skinLoader.LoadImage(skinFolder, settings.KeyPressedImage, KeyPressedImage, i);
        }

        ComboPosition = skinLoader.ArrToVec(settings.ComboPosition);
        
        AccuracyPosition = skinLoader.ArrToVec(settings.AccuracyPosition);
        AccuracyFormat = settings.AccuracyFormat;

        JudgeImage = new Texture[settings.JudgeImage.GetLength(0)];

        for (int i = 0; i != settings.JudgeImage.GetLength(0); i++)
        {
            skinLoader.LoadImage(skinFolder, settings.JudgeImage, JudgeImage, i);
        }

        Vector2 judgePosition = skinLoader.ArrToVec(settings.JudgePosition);
        Vector2 judgeScale = skinLoader.ArrToVec(settings.JudgeScale);
        JudgeRect = new Rect2(
            judgePosition,
            JudgeImage[0].GetSize().x * judgeScale.x,
            JudgeImage[0].GetSize().y * judgeScale.y
        );

        ErrorBarPosition = skinLoader.ArrToVec(settings.ErrorBarPosition);
    }
}

public class SkinLoader
{
    public struct SkinSettings
    {
        public int InputMode;

        public float[] Position;
        public float[] NoteSize;
        public float[] KeySize;

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

    public void LoadImage(string skinFolder, string[] namesArray, Texture[] imageArray, int index) 
    {   
        ImageTexture image = new ImageTexture();
        image.Load(skinFolder + namesArray[index]);
        imageArray[index] = image;
    }
}