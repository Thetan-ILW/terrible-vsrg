using Godot;
using System;
using System.IO;
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

    public Skin(SkinLoader.SkinSettings settings, SkinLoader sF, string skinFolder)
    {
        InputMode = settings.InputMode;

        Position = sF.ArrToVec(settings.Position);
        NoteSize = sF.ArrToVec(settings.NoteSize);
        KeySize = sF.ArrToVec(settings.KeySize);

        NoteImage = new Texture[settings.InputMode];
        KeyImage = new Texture[settings.InputMode];
        KeyPressedImage = new Texture[settings.InputMode];

        for (int i = 0; i != InputMode; i++)
        {
            sF.LoadImage(skinFolder, settings.NoteImage, NoteImage, i);
            sF.LoadImage(skinFolder, settings.KeyImage, KeyImage, i);
            sF.LoadImage(skinFolder, settings.KeyPressedImage, KeyPressedImage, i);
        }

        ComboPosition = sF.ArrToVec(settings.ComboPosition);
        
        AccuracyPosition = sF.ArrToVec(settings.AccuracyPosition);
        AccuracyFormat = settings.AccuracyFormat;
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
    {   //Тут грузим только ноты, кнопки, нажатые кнопки, хз какое название этой группы
        ImageTexture image = new ImageTexture();
        image.Load(skinFolder + namesArray[index]);
        imageArray[index] = image;
    }
}