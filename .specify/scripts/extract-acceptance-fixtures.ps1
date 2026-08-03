$base = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$specDir = Join-Path $base 'specs\001-contiguous-increasing-run\acceptance'
New-Item -ItemType Directory -Force -Path $specDir | Out-Null
$md = Get-Content -Raw (Join-Path $base 'requirements\code-test.md')
$pattern = '(?s)<summary>Test case (\d+)</summary>\s*Input\s*```\s*(.*?)```\s*Output\s*```\s*(.*?)```'
$count = 0
foreach ($match in [regex]::Matches($md, $pattern)) {
    $n = [int]$match.Groups[1].Value
    $input = $match.Groups[2].Value.Trim()
    $output = $match.Groups[3].Value.Trim()
    $id = '{0:D2}' -f $n
    Set-Content -Path (Join-Path $specDir "ac-$id-input.txt") -Value $input -NoNewline -Encoding utf8
    Set-Content -Path (Join-Path $specDir "ac-$id-expected.txt") -Value $output -NoNewline -Encoding utf8
    $count++
}
Write-Output "Extracted $count acceptance fixtures to $specDir"
