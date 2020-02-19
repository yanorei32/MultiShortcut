class ScheduledTask {
	int countdown = 0;
	string path;

	public string Path {
		get { return path; }
	}
	
	public bool Countdown() {
		return countdown-- == 0;
	}

	public ScheduledTask (string path, int delay) {
		this.countdown = delay;
		this.path = path;
	}
}

