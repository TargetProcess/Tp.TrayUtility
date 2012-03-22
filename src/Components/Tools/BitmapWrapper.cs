//  
// Copyright (c) 2005-2010 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace TpTrayUtility.Components.Tools
{
	public class BitmapWrapper : IDisposable
	{
		private readonly Bitmap _bitmap;

		public BitmapWrapper(Bitmap bitmap)
		{
			_bitmap = bitmap;
		}

		public void Save(string fileName, string mimeType, int quality)
		{			
			var encoder = GetEncoderInfo(mimeType);
			var fileStream = new FileStream(fileName, FileMode.Open);
			Save(fileStream, encoder, quality);			
		}

		public void Save(FileStream fileStream, ImageCodecInfo encoder, int quality)
		{
			var encoderParams = new EncoderParameters(2);
			encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);			
			encoderParams.Param[1] = new EncoderParameter(Encoder.ColorDepth, 24L);				
			_bitmap.Save(fileStream, encoder, encoderParams);
			fileStream.Close();
		}


		private static ImageCodecInfo GetEncoderInfo(String mimeType)
		{
			return ImageCodecInfo.GetImageEncoders().FirstOrDefault(t => t.MimeType == mimeType);
		}

		public void Dispose()
		{
			_bitmap.Dispose();
		}

		
	}
}