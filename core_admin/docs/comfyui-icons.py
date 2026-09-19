"""สร้างไอคอน widget สไตล์ SAM (glyph ขาวบนพื้นโปร่ง) ด้วย ComfyUI Z-Image Turbo แล้ว post-process เป็น PNG RGBA"""
import json, os, shutil, sys, time, urllib.request
from PIL import Image, ImageOps, ImageFilter
HOST = "http://127.0.0.1:8188"
COMFY_OUTPUT = r"C:\Users\ball\AppData\Local\Comfy-Desktop\ComfyUI-Shared\output"
SP = os.path.dirname(os.path.abspath(__file__))
RAW = os.path.join(SP, "icons", "raw"); OUT = os.path.join(SP, "icons", "png")
STYLE = ("flat minimalist vector icon, one single solid black pictogram centered on a plain pure white background, "
         "thick uniform black strokes, simple geometric shapes, no text, no letters, no numbers, no gradient, no shading, "
         "no background plate, no frame, no border, high contrast, wide empty white margin around the icon, "
         "clean 2D glyph in the style of a UI icon set. Subject: ")
def build(prompt, seed, prefix, size=1024):
    return {
        "28": {"class_type": "UNETLoader", "inputs": {"unet_name": "z_image_turbo_int8_convrot.safetensors", "weight_dtype": "default"}},
        "30": {"class_type": "CLIPLoader", "inputs": {"clip_name": "qwen_3_4b.safetensors", "type": "lumina2", "device": "default"}},
        "29": {"class_type": "VAELoader", "inputs": {"vae_name": "ae.safetensors"}},
        "11": {"class_type": "ModelSamplingAuraFlow", "inputs": {"model": ["28", 0], "shift": 3}},
        "27": {"class_type": "CLIPTextEncode", "inputs": {"clip": ["30", 0], "text": STYLE + prompt}},
        "33": {"class_type": "ConditioningZeroOut", "inputs": {"conditioning": ["27", 0]}},
        "13": {"class_type": "EmptySD3LatentImage", "inputs": {"width": size, "height": size, "batch_size": 1}},
        "3": {"class_type": "KSampler", "inputs": {"model": ["11", 0], "seed": seed, "steps": 8, "cfg": 1, "sampler_name": "res_multistep", "scheduler": "simple", "positive": ["27", 0], "negative": ["33", 0], "latent_image": ["13", 0], "denoise": 1}},
        "8": {"class_type": "VAEDecode", "inputs": {"samples": ["3", 0], "vae": ["29", 0]}},
        "9": {"class_type": "SaveImage", "inputs": {"images": ["8", 0], "filename_prefix": prefix}},
    }
def post(obj, path):
    req = urllib.request.Request(HOST + path, data=json.dumps(obj).encode(), headers={"Content-Type": "application/json"})
    return json.loads(urllib.request.urlopen(req, timeout=60).read())
def get(path): return json.loads(urllib.request.urlopen(HOST + path, timeout=60).read())

COLORS = {"white": (255, 255, 255), "navy": (0, 41, 90)}
def to_icon(src, dst, size=128, pad=0.12, color=(255, 255, 255)):
    im = Image.open(src).convert("L")
    # glyph ดำบนขาว → invert เป็น alpha, ยืด contrast แล้ว threshold นุ่ม ๆ
    im = ImageOps.invert(ImageOps.autocontrast(im, cutoff=1))
    a = im.point(lambda v: 0 if v < 60 else (255 if v > 190 else int((v - 60) * 255 / 130)))
    bbox = a.point(lambda v: 255 if v > 40 else 0).getbbox()
    if not bbox: raise SystemExit("empty icon " + src)
    a = a.crop(bbox)
    w, h = a.size; s = max(w, h); s = int(s * (1 + pad * 2))
    canvas = Image.new("L", (s, s), 0); canvas.paste(a, ((s - w) // 2, (s - h) // 2))
    canvas = canvas.resize((size, size), Image.LANCZOS)
    rgba = Image.new("RGBA", (size, size), color + (0,)); rgba.putalpha(canvas)
    rgba.save(dst, "PNG")

def main(jobs_file):
    jobs = json.load(open(jobs_file, encoding="utf-8"))
    os.makedirs(RAW, exist_ok=True); os.makedirs(OUT, exist_ok=True)
    ids = {}
    for j in jobs:
        r = post({"prompt": build(j["prompt"], j["seed"], "apicon_" + j["name"])}, "/prompt")
        ids[r["prompt_id"]] = j; print("queued", j["name"], flush=True)
    done, t0 = set(), time.time()
    while len(done) < len(ids) and time.time() - t0 < 3600:
        for pid, j in ids.items():
            if pid in done: continue
            h = get("/history/" + pid)
            if pid not in h: continue
            for out in h[pid].get("outputs", {}).values():
                for img in out.get("images", []):
                    src = os.path.join(COMFY_OUTPUT, img.get("subfolder", ""), img["filename"])
                    if os.path.exists(src):
                        raw = os.path.join(RAW, j["name"] + "_" + str(j["seed"]) + ".png"); shutil.move(src, raw)
                        for cn, cv in COLORS.items():
                            to_icon(raw, os.path.join(OUT, "%s_%s_%s.png" % (j["name"], j["seed"], cn)), color=cv)
                        print("DONE", j["name"], j["seed"], "%.0fs" % (time.time() - t0), flush=True)
            done.add(pid)
        if len(done) < len(ids): time.sleep(3)
if __name__ == "__main__": main(sys.argv[1])
