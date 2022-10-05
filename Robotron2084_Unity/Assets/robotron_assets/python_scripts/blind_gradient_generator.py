import pygame
import random

# Input parameters
out_fname = "gradient_blinds.png"
out_width = 1024
out_height = 16
num_panels = 20
base_color = (0, 0, 0)
color_choices = ((229, 25, 25), (255, 153, 94), (114, 0, 51), (165, 23, 35), (34, 249, 210), \
                 (0, 130, 47), (249, 92, 34), (0, 11, 137), (234, 124, 196), (65, 234, 65), \
                 (14, 107, 183), (249, 210, 34))
cur_color_index = 0
cur_color = color_choices[cur_color_index]


# Intermediate variables
surf = pygame.Surface((out_width, out_height))
panel_width = out_width / num_panels
cur_panel = 0
next_x = panel_width

for x in range(out_width):
    if x >= next_x:
        cur_panel += 1
        cur_color_index += 1
        cur_color = color_choices[cur_color_index % len(color_choices)]
        next_x += panel_width
       
    percent = (next_x - x) / panel_width
    
    color = tuple((int((1.0 - percent) * cur_color[i] + percent * base_color[i]) for i in range(3)))

    pygame.draw.line(surf, color, (x, 0), (x, out_height - 1))


pygame.image.save(surf, out_fname)
