import os
import os.path
import shutil

def main():
	shader_dir = os.path.join(os.getcwd(), 'Shaders')
	target_dir = os.path.join(os.getcwd(), 'CStrawberry3D\\bin\\Debug\\')
	for file in os.listdir(shader_dir):
		file_path = os.path.join(shader_dir, file)
		shutil.copy(file_path, target_dir)
	os.sys.exit(0)

if __name__ == '__main__':
	main()