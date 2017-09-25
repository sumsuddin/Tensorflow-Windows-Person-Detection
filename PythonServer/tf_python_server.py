import json, argparse, time

import tensorflow as tf

from flask import Flask, request
from flask_cors import CORS

import base64
import numpy as np
import cv2
import json

def load_graph(frozen_graph_filename):
    # We load the protobuf file from the disk and parse it to retrieve the 
    # unserialized graph_def
  detection_graph = tf.Graph()
  with detection_graph.as_default():
    od_graph_def = tf.GraphDef()
    with tf.gfile.GFile(frozen_graph_filename, 'rb') as fid:
      serialized_graph = fid.read()
      od_graph_def.ParseFromString(serialized_graph)
      tf.import_graph_def(od_graph_def, name='')

    return detection_graph

##################################################
# API part
##################################################
app = Flask(__name__)
cors = CORS(app)
@app.route("/api/predict", methods=['POST'])
def predict():
    start = time.time()
    
    data = request.data.decode("utf-8")
    if data == "":
        params = request.form
        input_str = json.loads(params['inputs'])
        threshold = float(json.loads(params['threshold']))
    else:
        params = json.loads(data)
        input_str = params['inputs']
        threshold = float(params['threshold'])

    input_str = base64.b64decode(input_str)    
    nparr = np.fromstring(input_str, np.uint8)
    image_np = cv2.imdecode(nparr, cv2.IMREAD_COLOR)
    
    # Expand dimensions since the model expects images to have shape: [1, None, None, 3]
    image_np_expanded = np.expand_dims(image_np, axis=0)


    ##################################################
    # Tensorflow part
    ##################################################
    (pred_boxes, pred_scores, pred_classes, pred_num_detections) = persistent_sess.run(
        [boxes, scores, classes, num_detections],
        feed_dict={image_tensor: image_np_expanded})

    ##################################################
    # END Tensorflow part
    ##################################################
    json_dict = {}
    detections = []
    for i in range(pred_scores.shape[1]) :
        if (pred_scores[0][i] > threshold) :
            tmp_dict = {}
            tmp_dict['score'] = str(pred_scores[0][i]);
            ymin, xmin, ymax, xmax = pred_boxes[0][i]
            box_dict = {}
            box_dict['xmin'] = str(xmin)
            box_dict['ymin'] = str(ymin)
            box_dict['xmax'] = str(xmax)
            box_dict['ymax'] = str(ymax)
            tmp_dict['box'] = box_dict
            tmp_dict['classId'] = str(int(pred_classes[0][i]))
            detections.append(tmp_dict)
    json_dict['detections'] = detections
    
    ret_data = json.dumps(json_dict)
    print("Time spent handling the request: %f" % (time.time() - start))
    
    return ret_data
##################################################
# END API part
##################################################

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--frozen_model_filename", default="results/frozen_model.pb", type=str, help="Frozen model file to import")
    parser.add_argument("--gpu_memory", default=.2, type=float, help="GPU memory per process")
    args = parser.parse_args()

    ##################################################
    # Tensorflow part
    ##################################################
    print('Loading the model')
    detection_graph = load_graph(args.frozen_model_filename)
    image_tensor = detection_graph.get_tensor_by_name('image_tensor:0')
    # Each box represents a part of the image where a particular object was detected.
    boxes = detection_graph.get_tensor_by_name('detection_boxes:0')
    # Each score represent how level of confidence for each of the objects.
    # Score is shown on the result image, together with the class label.
    scores = detection_graph.get_tensor_by_name('detection_scores:0')
    classes = detection_graph.get_tensor_by_name('detection_classes:0')
    num_detections = detection_graph.get_tensor_by_name('num_detections:0')    

    print('Starting Session, setting the GPU memory usage to %f' % args.gpu_memory)
    gpu_options = tf.GPUOptions(per_process_gpu_memory_fraction=args.gpu_memory)
    sess_config = tf.ConfigProto(gpu_options=gpu_options)
    persistent_sess = tf.Session(graph=detection_graph, config=sess_config)
    ##################################################
    # END Tensorflow part
    ##################################################

    print('Starting the API')
    app.run()
