//
//  UNOMetalViewDelegate.m
//

#import "UNOMetalViewDelegate.h"
#import "UNOWindow.h"
#import "UnoNativeMac.h"

static draw_fn_ptr draw;

inline draw_fn_ptr uno_get_draw_callback(void)
{
    return draw;
}

void uno_set_draw_callback(draw_fn_ptr p)
{
    draw = p;
}

@implementation UNOMetalViewDelegate
{
    id<MTLDevice> _device;
}

- (nonnull instancetype)initWithMetalKitView:(nonnull MTKView *)mtkView
{
    self = [super init];
    if (self)
    {
        _device = mtkView.device;
        self.queue = [_device newCommandQueue];
        
        mtkView.colorPixelFormat = MTLPixelFormatBGRA8Unorm;
        mtkView.depthStencilPixelFormat = MTLPixelFormatDepth32Float_Stencil8;
        mtkView.sampleCount = 1;
#if DEBUG
        NSLog(@"initWithMetalKitView: paused %s enableSetNeedsDisplay %s", mtkView.paused ? "true" : "false", mtkView.enableSetNeedsDisplay ? "true" : "false");
#endif
    }
    
    return self;
}

- (void)drawInMTKView:(nonnull MTKView *)view
{
#if DEBUG
    NSLog (@"drawInMTKView: %f %f", view.drawableSize.width, view.drawableSize.height);
#endif
    MTLRenderPassDescriptor *renderPassDescriptor = view.currentRenderPassDescriptor;
    if (renderPassDescriptor == nil)
    {
        return;
    }

    id<CAMetalDrawable> drawable = view.currentDrawable;

    CGSize size = view.drawableSize;
    // call managed code
    uno_get_draw_callback()((__bridge void*) view.window, size.width, size.height, (__bridge void*) drawable.texture);

    id<MTLCommandBuffer> commandBuffer = [self.queue commandBuffer];
    [commandBuffer presentDrawable:drawable];
    [commandBuffer commit];
}

- (void)mtkView:(nonnull MTKView *)view drawableSizeWillChange:(CGSize)size
{
    uno_get_resize_callback()((__bridge void*) view.window, size.width, size.height);
}

@end
