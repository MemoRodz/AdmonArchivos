window.convertImage = async function (imageDataUrl, targetFormat) {
    return new Promise((resolve, reject) => {
        var img = new Image();
        img.onload = function () {
            var canvas = document.createElement("canvas");
            canvas.width = img.width;
            canvas.height = img.height;
            var ctx = canvas.getContext("2d");
            ctx.drawImage(img, 0, 0);

            var mimeType = "image/png";
            if (targetFormat === "jpg" || targetFormat === "jpeg") mimeType = "image/jpeg";
            else if (targetFormat === "ico") mimeType = "image/x-icon";

            var dataUrl = canvas.toDataURL(mimeType);
            resolve(dataUrl);
        };
        img.onerror = function (err) {
            reject(err);
        };
        img.src = imageDataUrl;
    });
}
