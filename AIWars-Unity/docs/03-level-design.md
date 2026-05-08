# Level Design — Tutorial: Korean Peninsula

## Intent
**Player fantasy:** "I am a strategic commander who can dive into any unit and feel the buff."
**Pacing arc:** Exploration → first contact → possess introduction → small skirmish → resolution.
**New mechanic taught spatially:** the C/V possession toggle, taught by funneling the player toward a single isolated enemy in a sightline-controlled valley.

## Layout
| Zone | Function | Grey-box dimensions |
|---|---|---|
| Spawn plateau (Seoul) | Tutorial start, RTS camera intro | 80 × 80 m, flat |
| Central valley (38th parallel) | First skirmish, teaches selection + move order | 200 × 80 m, gentle slope |
| Northern overlook (Mt. Baekdu approach) | First possession beat — single elite enemy on rim, designed for FPS pickoff | 60 × 40 m, +20 m elevation |
| Eastern coast outpost | Optional reward — extra resource cache | 50 × 50 m, sea backdrop |
| Western corridor (toward Beijing exit) | Endgame zoom-out trigger to reveal globe | 40 × 200 m linear |

## Pacing chart
| Time | Activity | Tension |
|---|---|---|
| 0:00–1:00 | Camera pan, narration | Low |
| 1:00–2:30 | First selection + move | Low |
| 2:30–4:00 | First combat | Medium |
| 4:00–5:00 | Forced possession (Northern overlook) | High |
| 5:00–6:00 | Resolution — globe zoom-out | Low |

## Readability checklist
- [ ] Critical path is lit warm; optional cache is lit cool
- [ ] Northern overlook target is silhouetted against sky from spawn approach
- [ ] At least 2 cover positions on the overlook so possessed FPS combat has tactical options
- [ ] The C/V key prompt appears as a HUD overlay only after the player enters the overlook trigger

## Globe transition
At session end, camera dollies up to 5,000 m and a procedural sphere mesh fades in to reveal the full 3D Earth. This is the first long-term loop hook.
