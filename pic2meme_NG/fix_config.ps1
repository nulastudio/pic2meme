$path = $args[0]

$content = Get-Content -Raw -Encoding "UTF8" -Path $path

if (!$content) {
    $content = ""
}

$content = $content.Replace('<probing privatePath="lib;libs" xmlns="" />', '<probing privatePath="lib;libs" />')

Set-Content -Force -NoNewline -Encoding "UTF8" -Path $path -Value $content
