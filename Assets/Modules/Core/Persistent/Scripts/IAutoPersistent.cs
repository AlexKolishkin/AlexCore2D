using System;

public interface IAutoPersistent
{
	void OnLoaded();
}

public interface IManualPersistent : IAutoPersistent
{
	Type SaveType { get; }
	object GetSave();
	void LoadSave(object obj);
	void LoadDefaults();
}