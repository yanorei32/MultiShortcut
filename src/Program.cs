using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;

class Program {
#if english
	const string APPLICATION_NAME = "MultiShortcut";
#else
	const string APPLICATION_NAME = "マルチショートカット";
#endif

	static string getExeDir() {
		return Path.GetDirectoryName(
			Application.ExecutablePath
		);
	}

	static bool isSelfShortcut(string shortcutPath) {
		// ref: https://note.dokeep.jp/post/csharp-shortcut-file-load/
		var t = Type.GetTypeFromCLSID(
			new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")
		);

		dynamic shell = Activator.CreateInstance(t);

		var shortcut = shell.CreateShortcut(shortcutPath);

		var path = shortcut.TargetPath;

		Marshal.FinalReleaseComObject(shortcut);
		Marshal.FinalReleaseComObject(shell);

		return path == Application.ExecutablePath;
	}

	static void createShortcut(string dirName) {
		var t = Type.GetTypeFromCLSID(
			new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")
		);

		dynamic shell = Activator.CreateInstance(t);

		var shortcut = shell.CreateShortcut(
			Path.Combine(
				getExeDir(),
				string.Format("{0}.lnk", dirName)
			)
		);

		shortcut.IconLocation = Application.ExecutablePath + ",1";
		shortcut.TargetPath = Application.ExecutablePath;
		shortcut.Arguments = "\"" + dirName + "\"";
		shortcut.Save();

		Marshal.FinalReleaseComObject(shortcut);
		Marshal.FinalReleaseComObject(shell);
	}

	static void launchDirectory(string dirPath) {
		var startedCount = 0;

		foreach (
			var file in Directory.GetFileSystemEntries(dirPath, "*.url")
		) {
			Process.Start(file);
			++ startedCount;
		}

		foreach (
			var file in Directory.GetFileSystemEntries(dirPath, "*.website")
		) {
			Process.Start(file);
			++ startedCount;
		}

		foreach (
			var file in Directory.GetFileSystemEntries(dirPath, "*.lnk")
		) {
			if (isSelfShortcut(file))
				continue;

			Process.Start(file);
			++ startedCount;
		}

		if (startedCount != 0)
			return;

		MessageBox.Show(
			string.Format(
#if english
				"Launch shortcut not found in directory ({0})",
#else
				"フォルダ ({0}) 内にショートカットが存在しません。",
#endif
				Path.GetFileName(dirPath)
			),
			APPLICATION_NAME,
			MessageBoxButtons.OK,
			MessageBoxIcon.Information
		);
	}

	static void createShortcuts() {
		var createdCount = 0;

		foreach (
			var dirPath in Directory.GetDirectories(getExeDir())
		) {
			createShortcut(Path.GetFileName(dirPath));
			++ createdCount;
		}

		MessageBox.Show(
			string.Format(
#if english
				"{0} Shortcut(s) created",
#else
				"{0} つのショートカットが作成されました。",
#endif
				createdCount
			),
			APPLICATION_NAME,
			MessageBoxButtons.OK,
			MessageBoxIcon.Asterisk
		);
	}

	static void Main(string[] Args) {
		switch (Args.Length) {
			case 0:
				createShortcuts();

				return;

			case 1:
				var dirPath = Path.Combine(getExeDir(), Args[0]);

				if (!Directory.Exists(dirPath)) {
					MessageBox.Show(
						string.Format(
#if english
							"Directory not found: {0}",
#else
							"指定されたフォルダが存在しません: {0}",
#endif
							Args[0]
						),
						APPLICATION_NAME,
						MessageBoxButtons.OK,
						MessageBoxIcon.Hand
					);

					return;
				}

				launchDirectory(dirPath);

				return;

			default:
				MessageBox.Show(
#if english
					"Illegal arguments",
#else
					"引数が多すぎます",
#endif
					APPLICATION_NAME,
					MessageBoxButtons.OK,
					MessageBoxIcon.Hand
				);

				return;
		}
	}
}

