import fs from "fs";
import path from "path";
import { fileURLToPath } from "url";

const padZero = (num: number, length = 2): string => {
	return num.toString().padStart(length, "0");
};

const formatDate = (date: Date): string => {
	const year = date.getUTCFullYear();
	const month = padZero(date.getUTCMonth() + 1);
	const day = padZero(date.getUTCDate());
	const hours = padZero(date.getUTCHours());
	const minutes = padZero(date.getUTCMinutes());
	const seconds = padZero(date.getUTCSeconds());
	const milliseconds = padZero(date.getUTCMilliseconds(), 3);

	return `Z ${year}-${month}-${day} ${hours}:${minutes}:${seconds}.${milliseconds}`;
};

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

const packagePath = path.resolve(__dirname, "../package.json");
const versionPath = path.resolve(__dirname, "../public/version.json");
const manifestPath = path.resolve(__dirname, "../src/manifest.json");

// 读取 package.json 文件
const packageJson = JSON.parse(fs.readFileSync(packagePath, "utf-8"));
// 读取 version.json 文件
const versionJson = JSON.parse(fs.readFileSync(versionPath, "utf-8"));
// 读取 manifest.json 文件
const manifestJson = JSON.parse(fs.readFileSync(manifestPath, "utf-8"));

// 旧版本号
const oldVersion = packageJson.version as string;

// 根据.分割
const vArr = oldVersion.split(".");

// 获取版本号
let major = Number(vArr[0]);
let minor = Number(vArr[1]);
let patch = Number(vArr[2]);
patch += 1;

if (patch > 999) {
	// 第二位增加1
	minor += 1;
	patch = 0;
}

if (minor > 99) {
	// 第一位增加1
	major += 1;
	minor = 0;
}

// 新版本号
const newVersion = `${major}.${minor}.${patch}`;
const newVersionCode = `${major}${minor.toString().padStart(2, "0")}${patch.toString().padStart(3, "0")}`;

packageJson.version = newVersion;
manifestJson.versionName = newVersion;
manifestJson.versionCode = newVersionCode;

versionJson.version = newVersion;
versionJson.dateTime = formatDate(new Date());

// 写入 package.json 文件
fs.writeFileSync(packagePath, `${JSON.stringify(packageJson, null, "\t")}\n`, "utf-8");
// 写入 version.json 文件
fs.writeFileSync(versionPath, `${JSON.stringify(versionJson, null, "\t")}\n`, "utf-8");
// 写入 manifest.json 文件
fs.writeFileSync(manifestPath, `${JSON.stringify(manifestJson, null, 2)}\n`, "utf-8");

console.log(`
Update version to 
v${newVersion}
${newVersionCode}
`);
