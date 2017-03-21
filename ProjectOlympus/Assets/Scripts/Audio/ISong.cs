/// <summary>
///Anything that's considered a song, or represents a song must have a songInfo property.
///Not sure what else it will need but this is all that's needed for core functionality.
/// </summary>
public interface ISong<SongFormat>
{  
     SongInfo<SongFormat> songInfo { get; set; }		

}
