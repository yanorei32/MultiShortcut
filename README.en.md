# MultiShortcut

This application makes multiple-application-launch shortcut.

## How to use

1. Create a set directory with a custom name to this application `MultiShortcut(CreateShortcut).exe` directory.
2. Add shortcuts (`*.lnk`, `*.website`, `*.url`) to that directory.
3. Launch this application. (when launched, this application makes per-directory shortcuts.)
4. You can launch applications from a per-directory shortcut.

### other function

#### Delayed launch <small>v1.1+</small>
If you add prefix (like `+5s`) to application shortcut filename, it will be delayed launch.

### latest release

[Download](https://github.com/Yanorei32/MultiShortcut/releases)

[Download from BOOTH (for boost)](https://yanorei32.booth.pm/items/1835032)

## Bug report

* Twitter: [@yanorei32](https://twitter.com/yanorei32)
* GitHub: https://github.com/yanorei32/multishortcut/issues

## How to build (for developers)

### Depends

* Windows 10
* Cygwin
  * git
  * make
* Windows 10 SDK (10.0.18362)
* 7-zip

### Build

```bash
git clone https://github.com/yanorei32/multishortcut
cd multishortcut
make genzip # zip create (jp)
make clean # clean (jp)
make genzip LANG=en # zip create (en)
make clean LANG=en # clean (en)
```

## Idea, Logo, Production Cooperation
* [@FUMI23_VRC](https://twitter.com/intent/user?user_id=1217010323695128578)

