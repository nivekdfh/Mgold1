using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// code van PP
public static class MatrixExtensions
{
    public static Quaternion ExtractRotation(this Matrix4x4 matrix)
    {
        Vector3 forward;
        forward.x = matrix.m02;
        forward.y = matrix.m12;
        forward.z = matrix.m22;

        Vector3 upwards;
        upwards.x = matrix.m01;
        upwards.y = matrix.m11;
        upwards.z = matrix.m21;

        return Quaternion.LookRotation(forward, upwards);
    }

    public static Vector3 ExtractPosition(this Matrix4x4 matrix)
    {
        Vector3 position;
        position.x = matrix.m03;
        position.y = matrix.m13;
        position.z = matrix.m23;
        return position;
    }

}

public static class TransformExtensions
{
    public static void FromMatrix(this Transform transform, Matrix4x4 matrix)
    {
        transform.rotation = matrix.ExtractRotation();
        transform.position = matrix.ExtractPosition();
    }
} 


public class MATH: MonoBehaviour
{
    
    public Matrix4x4 rc;
    public Vector3  rotation;
    public Slider   rotationX, 
                    rotationY, 
                    rotationZ;
    public Vector3  translation;
    public Slider   translationX, 
                    translationY, 
                    translationZ;
    
// martix 4X4
    private void Start()
    {
        rc =    new Matrix4x4(
    		    new Vector4(1,0,0,0),
    		    new Vector4(0,1,0,0),
    		    new Vector4(0,0,1,0),
    		    new Vector4(0,0,0,1)
    	);  
        translation = new Vector3(0,0,0); 
    }   

    void Update()
    {
        rotation = new Vector3  (rotationX.value, 
                                rotationY.value, 
                                rotationZ.value);

        // var rc= RotationCube    (rotationX.value, 
        //                         rotationY.value, 
        //                         rotationZ.value,
        //                         translation);
        
        translation = new Vector3   (translationX.value, 
                                    translationY.value, 
                                    translationZ.value);

        rc = Product(translationMatrix(translation), rotationMatrix(rotation));

        transform.FromMatrix(rc);
    }

    // public Matrix4x4 RotationCube   (float centerX, 
    //                                 float centerY, 
    //                                 float centerZ,
    //                                 Vector3 position) 

    // return (rotationFromX(centerX) * rotationFromY(centerY) * rotationFromZ(centerZ)) * Translation(position);

    Matrix4x4 rotationMatrix(Vector3 rotation){
        return Product(Product(rotationFromX(rotation.x), rotationFromY(rotation.y)), rotationFromZ(rotation.z));
    } 

// Rotation X,Y,X
    public Matrix4x4 rotationFromX(float center)
    {
        return  new Matrix4x4(
                new Vector4(1, 0, 0, 0), 
                new Vector4(0, Mathf.Cos(center), Mathf.Sin(center), 0), 
                new Vector4(0, -Mathf.Sin(center), Mathf.Cos(center), 0),
                new Vector4(0, 0, 0, 1)
                );
    }

    public Matrix4x4 rotationFromY(float center)
    {
        return  new Matrix4x4(
                new Vector4(Mathf.Cos(center), 0, -Mathf.Sin(center), 0),
                new Vector4(0, 1, 0, 0),
                new Vector4(Mathf.Sin(center), 0, Mathf.Cos(center), 0),
                new Vector4(0, 0, 0, 1)
                );
    }

    public Matrix4x4 rotationFromZ(float center)
    {
        return  new Matrix4x4(
                new Vector4(Mathf.Cos(center), Mathf.Sin(center), 0, 0),
                new Vector4(-Mathf.Sin(center), Mathf.Cos(center), 0, 0),
                new Vector4(0, 0, 1, 0),
                new Vector4(0, 0, 0, 1)
                );
    }

    //this is based on a formula that's in the slides. It's just the product of two matrices. You can find this online.
    static Matrix4x4 Product(Matrix4x4 one, Matrix4x4 two){
        Matrix4x4 temp;
        temp.m00 =  (one.m00 * two.m00 + one.m01 * two.m10 + one.m02 *  two.m20 + one.m03 *  two.m30);
        temp.m01 =  (one.m00 * two.m01 + one.m01 * two.m11 + one.m02 *  two.m21 + one.m03 *  two.m31);
        temp.m02 =  (one.m00 * two.m02 + one.m01 * two.m12 + one.m02 *  two.m22 + one.m03 *  two.m32);
        temp.m03 =  (one.m00 * two.m03 + one.m01 * two.m13 + one.m02 *  two.m23 + one.m03 *  two.m33);
        temp.m10 =  (one.m10 * two.m00 + one.m11 * two.m10 + one.m12 *  two.m20 + one.m13 *  two.m30);
        temp.m11 =  (one.m10 * two.m01 + one.m11 * two.m11 + one.m12 *  two.m21 + one.m13 *  two.m31);
        temp.m12 =  (one.m10 * two.m02 + one.m11 * two.m12 + one.m12 *  two.m22 + one.m13 *  two.m32);
        temp.m13 =  (one.m10 * two.m03 + one.m11 * two.m13 + one.m12 *  two.m23 + one.m13 *  two.m33);
        temp.m20 =  (one.m20 * two.m00 + one.m21 * two.m10 + one.m22 *  two.m20 + one.m23 *  two.m30);
        temp.m21 =  (one.m20 * two.m01 + one.m21 * two.m11 + one.m22 *  two.m21 + one.m23 *  two.m31);
        temp.m22 =  (one.m20 * two.m02 + one.m21 * two.m12 + one.m22 *  two.m22 + one.m23 *  two.m32);
        temp.m23 =  (one.m20 * two.m03 + one.m21 * two.m13 + one.m22 *  two.m23 + one.m23 *  two.m33);
        temp.m30 =  (one.m30 * two.m00 + one.m31 * two.m10 + one.m32 *  two.m20 + one.m33 *  two.m30);
        temp.m31 =  (one.m30 * two.m01 + one.m31 * two.m11 + one.m32 *  two.m21 + one.m33 *  two.m31);
        temp.m32 =  (one.m30 * two.m02 + one.m31 * two.m12 + one.m32 *  two.m22 + one.m33 *  two.m32);
        temp.m33 =  (one.m30 * two.m03 + one.m31 * two.m13 + one.m32 *  two.m23 + one.m33 *  two.m33);
      return temp;
    }

// translation X,Y,Z
    public Matrix4x4 translationMatrix(Vector3 translation)
    {
        return  new Matrix4x4(new Vector4(1, 0, 0, 0),
                             new Vector4(0, 1, 0, 0),
                             new Vector4(0, 0, 1, 0),
                             new Vector4(translation.x, translation.y, translation.z, 1));                         
    }


}
