const { PI, cos, sin } = Math

// 伯努利双纽线
export function lemniscateMove(r: number = 30, speed: number = 0.02) {
  let t = 0 // 0到2PI
  return function () {
    t += speed
    if (t >= 2 * PI) t = 0
    let s = sin(t)
    let c = cos(t)
    let x = (r * s) / (1 + c * c)
    let y = (r * s * c) / (1 + c * c)
    return { x, y }
  }
}
