using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class Watermark : MonoBehaviour
{
    private string FilePath => Path.Combine(Application.dataPath, "key.txt");
    private void Awake()
    {
        try
        {
            if (!File.Exists(FilePath))
            {
                var stream = File.Create(FilePath);
                stream.Close();
                stream.Dispose();
                transform.localScale = 8 * Vector3.one;
                return;
            }

            var content = File.ReadAllText(FilePath);
            if (content.Contains("2021multitap2021"))
            {
                gameObject.SetActive(false);
                return;
            }
            if (DateTime.TryParse(content,out var date))
            {
                var timeSpan = DateTime.Now - date;
                if (timeSpan > TimeSpan.Zero)
                {
                    if (timeSpan.TotalDays < 7)
                    {
                        transform.localScale = Vector3.one;
                    }
                    else
                    {
                        transform.localScale = Vector3.one * (1+ Mathf.Clamp(((float)timeSpan.TotalDays - 7) / 2f, 0, 7));
                    }
                }
                else
                {
                    transform.localScale = Vector3.one;
                }
            }
            else
            {
                transform.localScale = 8 * Vector3.one;
            }
        }
        catch (Exception e)
        {
            transform.localScale = Vector3.one;
            throw;
        }

    }
}
